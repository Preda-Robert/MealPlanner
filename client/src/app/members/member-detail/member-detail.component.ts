import {
  Component, signal, inject, OnInit
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from '../../_models/member';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MemberService } from '../../_services/member.service';
import { RecipeService } from '../../_services/recipe.service';
import { MealPlanService } from '../../_services/meal-plan.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  standalone: true,
  imports: [CommonModule, TimeagoModule, PaginationModule, FormsModule]
})
export class MemberDetailComponent implements OnInit {
  member = signal<Member | null>(null);

  recipeSearchQuery = '';
  mealPlanSearchQuery = '';

  memberService = inject(MemberService);
  recipeService = inject(RecipeService);
  mealPlanService = inject(MealPlanService);
  route = inject(ActivatedRoute);
  router = inject(Router);

  ngOnInit(): void {
    this.memberService.loadMember().subscribe({
      next: (member: Member | null) => {
        this.member.set(member);
        if (member?.userName) {
          this.loadMemberRecipes();
          this.loadMemberMealPlans();
        }
      },
      error: error => {
        console.error('Failed to load member:', error);
      }
    });
  }

  loadMemberRecipes() {
    if (!this.member()?.userName) return;
    this.recipeService.getRecipes(this.recipeSearchQuery);
  }

  loadMemberMealPlans() {
    if (!this.member()?.userName) return;
    this.mealPlanService.getMealPlans(this.mealPlanSearchQuery);
  }

  onRecipePageChanged(event: any) {
    if (this.recipeService.recipeParams().pageNumber !== event.page) {
      this.recipeService.recipeParams().pageNumber = event.page;
      this.loadMemberRecipes();
    }
  }

  onMealPlanPageChanged(event: any) {
    if (this.mealPlanService.mealPlanParams().pageNumber !== event.page) {
      this.mealPlanService.mealPlanParams().pageNumber = event.page;
      this.loadMemberMealPlans();
    }
  }

  onRecipeSearch() {
    this.recipeService.recipeParams().pageNumber = 1;
    this.loadMemberRecipes();
  }

  onMealPlanSearch() {
    this.mealPlanService.mealPlanParams().pageNumber = 1;
    this.loadMemberMealPlans();
  }

  toggleLike() {
    // Implement like toggling if needed
  }

  hasLiked(): boolean {
    return false;
  }
}
