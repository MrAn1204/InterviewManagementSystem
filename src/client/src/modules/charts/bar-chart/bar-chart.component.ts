import {
  Component,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  EventEmitter,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { log } from 'console';
Chart.register(...registerables);

@Component({
  selector: 'app-bar-chart',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './bar-chart.component.html',
  styleUrl: './bar-chart.component.css',
})
export class BarChartComponent implements OnInit, OnChanges {
  public months = [
    { label: 'Tháng 1', value: 1 },
    { label: 'Tháng 2', value: 2 },
    { label: 'Tháng 3', value: 3 },
    { label: 'Tháng 4', value: 4 },
    { label: 'Tháng 5', value: 5 },
    { label: 'Tháng 6', value: 6 },
    { label: 'Tháng 7', value: 7 },
    { label: 'Tháng 8', value: 8 },
    { label: 'Tháng 9', value: 9 },
    { label: 'Tháng 10', value: 10 },
    { label: 'Tháng 11', value: 11 },
    { label: 'Tháng 12', value: 12 },
  ];
  @ViewChild('chartCanvas', { static: true }) chartRef!: ElementRef;
  labelData: string[] = ['Week1', 'Week2', 'Week3', 'Week4'];
  @Input() realData: number[] = [];
  monthControl = new FormControl(new Date().getMonth() + 1);
  years: number[] = [];
  yearControl = new FormControl(new Date().getFullYear());
  @Input() label: string = 'Data';
  @Input() title: string = 'Chart';
  myChart!: Chart;
  @Output() monthYearChange = new EventEmitter<{
    month: number;
    year: number;
  }>();

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    this.renderChart();
  }
  ngOnInit(): void {
    this.initYears();
    this.renderChart();
  }

  renderChart() {
    if (this.myChart) {
      this.myChart.destroy();
    }

    this.myChart = new Chart(this.chartRef.nativeElement, {
      type: 'bar',
      data: {
        labels: this.labelData,
        datasets: [{ label: this.label, data: this.realData }],
      },
      options: {},
    });
  }

  initYears() {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 10; i <= currentYear; i++) {
      this.years.push(i);
    }
  }

  emitMonthYearChange() {
    console.log('okok');

    this.monthYearChange.emit({
      month: this.monthControl.value ?? new Date().getMonth(),
      year: this.yearControl.value ?? new Date().getFullYear(),
    });
  }
}
