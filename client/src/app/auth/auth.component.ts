import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../_services/authentication.service';
import { Router, RouterLink } from '@angular/router';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { CommonModule } from '@angular/common';
import { GoogleApiService } from '../_services/google-api.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css',
  imports: [ReactiveFormsModule, TextInputComponent, RouterLink, CommonModule]
})
export class AuthComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authenticationService = inject(AuthenticationService);
  private router = inject(Router);
  private googleService = inject(GoogleApiService);

  isLoginMode = true;
  authForm!: FormGroup;
  validationErrors: string[] | undefined;

  ngOnInit(): void {
    this.buildForm();
  }

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.buildForm();
  }

  buildForm() {
    if (this.isLoginMode) {
      this.authForm = this.fb.group({
        usernameOrEmail: ['', Validators.required],
        password: ['', Validators.required]
      });
      return;
    }
      this.authForm = this.fb.group({
        username: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]],
        displayName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]],
        confirmPassword: ['', [Validators.required, this.matchValues('password')]]
      });

      this.authForm.controls['password'].valueChanges.subscribe(() => {
        this.authForm.controls['confirmPassword'].updateValueAndValidity();
      });
  }

  matchValues(matchTo: string) {
    return (control: any) =>
      control?.value === control?.parent?.get(matchTo)?.value ? null : { isMatching: true };
  }

  loginWithGoogle() {
    this.googleService.configure().subscribe(idToken => {
      if (idToken) {
        this.authenticationService.googleLogin(idToken).subscribe({
          next: () => {
            this.router.navigateByUrl('/');
          },
          error: error => {
            this.validationErrors = error.error ? [error.error] : ['Google login failed'];
          }
        });
      }
    });
  }

  onSubmit() {
    if (this.isLoginMode) {
      const credentials = {
        usernameOrEmail: this.authForm.value.usernameOrEmail,
        password: this.authForm.value.password
      };
      this.authenticationService.login(credentials).subscribe({
        next: () => this.router.navigate(['/members']),
        error: err => this.validationErrors = err
      });
    } else {
      this.authenticationService.register(this.authForm.value).subscribe({
        next: () => this.router.navigate(['/members']),
        error: err => this.validationErrors = err
      });
    }
  }
}
