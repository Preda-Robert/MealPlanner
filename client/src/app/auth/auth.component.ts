import { Component, effect, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../_services/authentication.service';
import { Router, RouterLink } from '@angular/router';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { CommonModule } from '@angular/common';
import { GoogleApiService } from '../_services/google-api.service';
import { User } from '../_models/user';

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
    console.log('loginWithGoogle');
    this.googleService.loginWithGoogle();
  }

  onSubmit() {
    if (this.isLoginMode) {
      const credentials = {
        usernameOrEmail: this.authForm.value.usernameOrEmail,
        password: this.authForm.value.password
      };
      this.authenticationService.login(credentials).subscribe({
        next: (user) => {
          console.log('User:', user);
          if (user && user.emailConfirmed === false) {
            console.log('User is not verified');
            sessionStorage.setItem('pendingLoginCredentials', JSON.stringify(credentials));
            sessionStorage.setItem('verificationUserId', user.id.toString());
            this.router.navigate(['/verify-email']);
          } else if (user && user.emailConfirmed === true && user.hasDoneSetup === true) {
            this.router.navigate(['/member-details']);
          }
          else if (user && user.emailConfirmed === true && user.hasDoneSetup === false) {
            this.router.navigate(['/setup-selection']);
          }
          if(typeof user === 'undefined')
          {
            this.validationErrors = ['Invalid username or password'];
          }

        },
        error: err => {
        if (err?.status === 401 || err?.status === 400) {
            this.validationErrors = ['Invalid username or password'];
          } else {
            const fallback = err?.error?.message || err?.message || 'Something went wrong';
            this.validationErrors = [fallback];
          }
        }
      });
    } else {
      this.authenticationService.register(this.authForm.value).subscribe({
        next: (registerResponse) => {
          if(registerResponse && registerResponse.requireEmailVerification !== false)
          {
            sessionStorage.setItem('pendingLoginCredentials', JSON.stringify(this.authForm.value));
            sessionStorage.setItem('verificationUserId', registerResponse.userId.toString());
          }
          this.validationErrors = undefined;
          this.router.navigate(['/verify-email']);
        },
        error: err => {
          if (err?.status === 401 || err?.status === 400) {
              this.validationErrors = ['Invalid username or password'];
            } else {
              const fallback = err?.error?.message || err?.message || 'Something went wrong';
              this.validationErrors = [fallback];
            }
          }
      });
    }
  }
}
