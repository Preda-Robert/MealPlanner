// client/src/app/verification/verification.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-verification',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.css']
})
export class VerificationComponent implements OnInit {
  verificationForm!: FormGroup;
  loading = false;
  userId: number | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private toastr: ToastrService,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.verificationForm = this.fb.group({
      code: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(6),
        Validators.pattern('^[0-9]*$')
      ]]
    });

    // Get userId from session storage
    const storedUserId = sessionStorage.getItem('verificationUserId');
    console.log('Stored verification userId:', storedUserId);

    if (storedUserId) {
      this.userId = parseInt(storedUserId, 10);
      console.log('Using userId for verification:', this.userId);
    } else {
      console.error('No userId found in session storage');
      this.toastr.error('Verification session expired. Please try logging in again.');
      this.router.navigateByUrl('/auth');
    }
  }

  submitVerification(): void {
    if (this.verificationForm.valid && this.userId) {
      this.loading = true;

      const verificationData = {
        id: this.userId,
        code: this.verificationForm.get('code')?.value
      };
      console.log('Verification data:', verificationData);
      this.authenticationService.verifyEmailCode(verificationData).subscribe({
        next: () => {
          this.loading = false;
          sessionStorage.removeItem('verificationUserId');
          this.toastr.success('Email verified successfully!');

          // Important: Log the user in after verification
          const storedLoginInfo = sessionStorage.getItem('pendingLoginCredentials');
          if (storedLoginInfo) {
            const loginCredentials = JSON.parse(storedLoginInfo);
            this.authenticationService.login(loginCredentials).subscribe({
              next: () => {
                // Clear stored credentials
                sessionStorage.removeItem('pendingLoginCredentials');
                // Redirect to home page
                this.router.navigateByUrl('/');
              },
              error: () => {
                // If automatic login fails, go to login page
                this.router.navigateByUrl('/auth');
              }
            });
          } else {
            // If no stored credentials, go to login page
            this.router.navigateByUrl('/auth');
          }
        },
        error: () => {
          this.loading = false;
          this.errorMessage = 'Invalid verification code. Please try again.';
          this.verificationForm.get('code')?.reset();
        }
      });
    }
  }

  resendCode(): void {
    if (this.userId) {
      this.loading = true;

      this.authenticationService.resendVerificationCode(this.userId).subscribe({
        next: () => {
          this.loading = false;
          this.toastr.info('A new verification code has been sent to your email');
        },
        error: (error: any) => {
          this.loading = false;
          console.error('Resend error in component:', error);
          this.toastr.error('Failed to resend verification code');
        },
        complete: () => {
          this.loading = false;
        }
      });
    } else {
      this.toastr.error('User ID not found');
    }
  }

  goToLogin(): void {
    sessionStorage.removeItem('verificationUserId');
    this.router.navigateByUrl('/auth');
  }
}
