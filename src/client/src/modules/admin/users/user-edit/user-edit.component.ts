import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Department } from '../../../../models/data-for-input/department.model';
import { UserService } from '../../../../services/user/user.service';
import { DepartmentService } from '../../../../services/department/department.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-edit',
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.css'
})
export class UserEditComponent implements OnInit {
  userId!: number;
  userForm: FormGroup;
  departments: Department[] = [];
  roles = [ 'RECRUITER', 'INTERVIEWER', 'MANAGER'];
  selectedRoles: string[] = [];
  isLoading = true;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly fb: FormBuilder,
    private readonly userService: UserService,
    private readonly departmentService: DepartmentService,
    private readonly toastr: ToastrService,
    private readonly router: Router
  ) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9_@.]{3,30}$/)]],
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      dob: [''],
      address: [''],
      phoneNumber: ['', [Validators.pattern(/^[0-9]{10,11}$/)]],
      gender: ['', Validators.required],
      roles: [[], Validators.required],
      departmentId: ['', Validators.required],
      note: ['']
    });
  }

  ngOnInit(): void {
    this.userId = +this.route.snapshot.paramMap.get('id')!;

    this.loadDepartments();
    this.loadUserData();
  }

  private loadUserData(): void {
    this.userService.getById(this.userId).subscribe({
      next: (user) => {
        this.selectedRoles = user.roles;
        this.userForm.patchValue({
          ...user,
          gender: user.gender?.toLowerCase()
        });
        this.isLoading = false;
      },
      error: (err) => {
        this.toastr.error('Failed to load user data');
        this.router.navigate(['/admin/users']);
      }
    });
  }


  private loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (data) => this.departments = data,
      error: (err) => console.error('Error loading departments:', err)
    });
  }

  private formatDateForInput(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toISOString().slice(0, 16);
  }

  onRoleChange(role: string, isChecked: boolean): void {
    const rolesControl = this.userForm.get('roles');
    const currentRoles: string[] = rolesControl?.value || [];

    if (isChecked) {
      currentRoles.push(role);
    } else {
      const index = currentRoles.indexOf(role);
      if (index > -1) currentRoles.splice(index, 1);
    }

    rolesControl?.setValue(currentRoles);
    rolesControl?.markAsTouched();
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      this.markFormGroupTouched(this.userForm);
      return;
    }

    const formData = { ...this.userForm.value };

    this.userService.update(this.userId, formData).subscribe({
      next: () => {
        this.toastr.success('User updated successfully');
        this.router.navigate(['/admin/users']);
      },
      error: (err) => {
        this.toastr.error('Failed to update user');
        console.error(err);
      }
    });
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
