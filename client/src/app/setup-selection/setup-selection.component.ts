import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { Router } from '@angular/router';
import { AllergyService } from '../_services/allergy.service';
import { MemberService } from '../_services/member.service';
import { DietType } from '../_models/dietType';
import { ServingType } from '../_models/servingType';
import { ServingTypeService } from '../_services/serving-type.service';
import { DietTypeService } from '../_services/diet-type.service';
import { DietPreferenceService } from '../_services/diet-preference.service';
import { SaveDietPreference } from '../_models/saveDietPreference';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-setup-selection',
  standalone: true,
  imports: [CommonModule, FormsModule, PaginationModule],
  templateUrl: './setup-selection.component.html',
})
export class SetupSelectionComponent implements OnInit {
  allergyService = inject(AllergyService);
  memberService = inject(MemberService);
  authenticationService = inject(AuthenticationService);
  router = inject(Router);
  servingTypeService = inject(ServingTypeService);
  dietTypeService = inject(DietTypeService);
  dietaryPreferenceService = inject(DietPreferenceService);

  // Initialize with undefined instead of null
  selectedDietTypeId: number | undefined;
  selectedServingTypeId: number | undefined;
  selectedAllergies = new Set<number>();

  dietTypes: DietType[] = [];
  servingTypes: ServingType[] = [];

  ngOnInit(): void {
    this.allergyService.getAllergies();
    this.loadDietTypes();
    this.loadServingTypes();
  }

  loadDietTypes() {
    this.dietTypeService.getDietTypes().subscribe({
      next: (types) => {
        this.dietTypes = types;
        console.log('Diet types loaded:', types);
        const anyMissingId = types.some(type => type.id === undefined);
        if (anyMissingId) {
          console.warn('Some diet types are missing IDs!');
        }
        if (this.dietTypes.length > 0) {
          this.selectedDietTypeId = this.dietTypes[0].id;
        }
      },
      error: (err) => console.error('Error loading diet types:', err)
    });
  }

  loadServingTypes() {
    this.servingTypeService.getServingTypes().subscribe({
      next: (types) => {
        this.servingTypes = types;
        console.log('Serving types loaded:', types);
        const anyMissingId = types.some(type => type.id === undefined);
        if (anyMissingId) {
          console.warn('Some serving types are missing IDs!');
        }
        if (this.servingTypes.length > 0) {
          this.selectedServingTypeId = this.servingTypes[0].id;
        }
      },
      error: (err) => console.error('Error loading serving types:', err)
    });
  }

  onPageChanged(event: any) {
    if (this.allergyService.allergyParams().pageNumber !== event.page) {
      this.allergyService.allergyParams().pageNumber = event.page;
      this.allergyService.getAllergies();
    }
  }

  submitDietaryPreferences() {
    console.log('Submitting dietary preferences...');
    if (!this.selectedDietTypeId || !this.selectedServingTypeId) {
      console.error('Diet type or serving type not selected');
      return;
    }

    console.log('Selected Diet Type ID:', this.selectedDietTypeId);
    console.log('Selected Serving Type ID:', this.selectedServingTypeId);

    const dietPreference: SaveDietPreference = {
      userId: this.authenticationService.currentUser()?.id || 0,
      dietTypeId: this.selectedDietTypeId,
      servingTypeId: this.selectedServingTypeId,
      allergies: Array.from(this.selectedAllergies),
    };

    this.dietaryPreferenceService.saveDietPreferences(dietPreference).subscribe({
      next: () => {
        this.router.navigate(['/home']);
        this.authenticationService.doneSelection(dietPreference);
      },
      error: (error) => {
        console.error('Failed to save preferences', error);
      }
    });
  }

  // Modified to use index-based selection instead of ID-based selection
  toggleAllergySelection(id: number) {
    this.selectedAllergies.has(id)
      ? this.selectedAllergies.delete(id)
      : this.selectedAllergies.add(id);
  }

  isSelected(id: number): boolean {
    return this.selectedAllergies.has(id);
  }

  findDietTypeIndexById(id: number | undefined): number {
    if (id === undefined) return -1;
    return this.dietTypes.findIndex(type => type.id === id);
  }

  findServingTypeIndexById(id: number | undefined): number {
    if (id === undefined) return -1;
    return this.servingTypes.findIndex(serving => serving.id === id);
  }
}
