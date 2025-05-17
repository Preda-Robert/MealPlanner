import { Component, OnInit } from '@angular/core';
import { MealPlanService } from '../_services/meal-plan.service';
import { MealPlan } from '../_models/mealPlan';
import { addDays, startOfWeek, endOfWeek } from 'date-fns';
import { CommonModule } from '@angular/common';
import { NgIf, NgFor, DatePipe } from '@angular/common';

@Component({
  selector: 'app-mealplans',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor, DatePipe],
  templateUrl: './mealplans.component.html'
})
export class MealPlansComponent implements OnInit {
  currentStart!: Date;
  currentEnd!: Date;
  mealPlan?: MealPlan;
  error?: string;

  constructor(private mealPlanService: MealPlanService) {}

  ngOnInit(): void {
    this.setCurrentWeek();
    this.loadMealPlan();
  }

  setCurrentWeek(date = new Date()) {
    this.currentStart = startOfWeek(date, { weekStartsOn: 1 }); // Monday
    this.currentEnd = endOfWeek(date, { weekStartsOn: 1 });
  }

  loadMealPlan() {
    this.mealPlanService.getByDateRange(this.currentStart, this.currentEnd).subscribe({
      next: plan => {
        this.mealPlan = plan;
        this.error = undefined;
      },
      error: err => {
        this.mealPlan = undefined;
        this.error = err.error || 'Meal plan not found.';
      }
    });
  }

  prevWeek() {
    this.setCurrentWeek(addDays(this.currentStart, -7));
    this.loadMealPlan();
  }

  nextWeek() {
    this.setCurrentWeek(addDays(this.currentStart, 7));
    this.loadMealPlan();
  }
}
