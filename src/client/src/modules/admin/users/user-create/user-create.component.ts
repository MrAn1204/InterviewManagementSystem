import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../../services/user/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgFor, NgIf } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DepartmentService } from '../../../../services/department/department.service';
import { Department } from '../../../../models/Department';

@Component({
  selector: 'app-user-create',
  imports: [RouterLink, FormsModule, NgIf, ReactiveFormsModule, NgFor],
  templateUrl: './user-create.component.html',
  styleUrl: './user-create.component.css'
})
export class UserCreateComponent implements OnInit{
  userForm: FormGroup;
  departments: Department[] = [];
  roles = ['RECRUITER', 'INTERVIEWER', 'MANAGER'];
  selectedRoles: string[] = [];

  constructor(
    private fb: FormBuilder,
    private departmentService: DepartmentService,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService,
  ) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9_@.]{3,30}$/)]],
      email: ['', [Validators.required, Validators.email]],
      fullName: ['', Validators.required],
      dob: [''],
      address: [''],
      gender: ['', Validators.required],
      isActive: ['', Validators.required],
      phoneNumber: ['', Validators.pattern(/^[0-9]{10,11}$/)],
      departmentId: ['', Validators.required],
      roles: [[], Validators.required],
      note: ['']
    });
  }

  ngOnInit() {
    this.loadDepartments();
  }
  private loadDepartments() {
    this.departmentService.getAllDepartments().subscribe({
      next: (data) => this.departments = data,
      error: (err) => console.error('Error loading departments:', err)
    });
  }

  onSubmit() {
    if (this.userForm.invalid) {
      this.markFormGroupTouched(this.userForm);
      return;
    }

    const userData = this.userForm.value;
    this.userService.createUser(userData).subscribe({
      next: (response) => {
        this.toastr.success('User created successfully!', 'Success');
        this.router.navigate(['/admin/users']);
      },
      error: (err) => {
        this.toastr.error('Error creating user. Please try again.', 'Error');
        console.error('Create user error:', err);
      }
    });
  }

  onRoleChange(role: string, isChecked: boolean) {
    const rolesControl = this.userForm.get('roles');
    const currentRoles: string[] = rolesControl?.value || [];

    if (isChecked) {
      currentRoles.push(role);
    } else {
      const index = currentRoles.indexOf(role);
      if (index > -1) {
        currentRoles.splice(index, 1);
      }
    }
    rolesControl?.setValue(currentRoles);
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}
