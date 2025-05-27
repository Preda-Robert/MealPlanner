import { Component, effect, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../_services/authentication.service';
import { Router, RouterLink } from '@angular/router';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { CommonModule } from '@angular/common';
import { GoogleApiService } from '../_services/google-api.service';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';

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
  toastr = inject(ToastrService);

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
    this.googleService.oAuthDiscovery();
  }

  onSubmit() {
    if (this.isLoginMode) {
      this.login();
    } else {
      this.register();
    }
  }

  login() {
    if (this.authForm.valid) {
      const credentials = {
        usernameOrEmail: this.authForm.value.usernameOrEmail,
        password: this.authForm.value.password,
      };

      this.authenticationService.login(credentials).subscribe({
        next: () => {
          if (this.authenticationService.currentUser()?.hasDoneSetup === false) {
            this.router.navigate(['/setup-selection']);
          }
          else
          {
            this.router.navigate(['/']);
          }
        },
        error: error => {
          if (error && error.requiresVerification) {
            sessionStorage.setItem('pendingLoginCredentials', JSON.stringify(credentials));
            this.router.navigateByUrl('/verify-email');
          } else {
            this.validationErrors = typeof error === 'string' ? [error] : ['Login failed'];
          }
        }
      });
    }
  }

  register() {
    if (this.authForm.valid) {
      this.validationErrors = undefined;

      console.log('Submitting registration form:', this.authForm.value);

      this.authenticationService.register(this.authForm.value).subscribe({
        next: response => {
          console.log('Registration response:', response);

          if (response.requireEmailVerification) {
            console.log('Email confirmation required, userId:', response.userId);

            if (response.userId) {
              sessionStorage.setItem('verificationUserId', response.userId.toString());
              this.toastr.info('Please verify your email. Check your inbox for a verification code.');

              setTimeout(() => {
                this.router.navigateByUrl('/verify-email');
              }, 500);
            } else {
              console.error('No userId in response:', response);
              this.toastr.error('Registration completed but verification is not available');
            }
          } else {
            this.toastr.success('Registration successful!');
            if(this.authenticationService.currentUser()?.hasDoneSetup === false) {
              this.router.navigateByUrl('/setup-selection');
            }
            this.router.navigateByUrl('/');
          }
        },
        error: error => {
          console.error('Registration error:', error);

          if (typeof error === 'string') {
            this.validationErrors = [error];
          } else if (Array.isArray(error)) {
            this.validationErrors = error;
          } else {
            this.validationErrors = ['Registration failed. Please try again.'];
          }
        }
      });
    }
  }
}
