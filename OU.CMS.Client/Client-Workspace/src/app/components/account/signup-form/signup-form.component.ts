import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/api/account.service';
import { SignUpDto } from 'src/app/shared/models/AccountModels';
import { UserType } from 'src/app/shared/enums/UserType';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, NgForm } from '@angular/forms';

@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.css']
})
export class SignupFormComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  signUpCredentials: SignUpDto = new SignUpDto();
  @Input('isCandidateSignup') isCandidateSignup: boolean;
  userTypeTag: string;

  takingDetails: boolean;

  ngOnInit(): void {
    this.resetForm();
    this.signUpCredentials.UserType = this.isCandidateSignup ? UserType.Candidate : UserType.Management;
    this.userTypeTag = this.isCandidateSignup ? 'Candidate' : 'Company Manager';
    this.takingDetails = false;
  }

  signUp(form: SignUpDto) {
    this.accountService.register(form).subscribe(
      (result) => {
        this.toastr.success('You have signed up successfully! Please proceed to sign in.', 'Sign up Successful!');
        if (form.UserType === UserType.Candidate) {
          this.router.navigate(['/candidate-signin']);
        } else {
          this.router.navigate(['/companyManager-signin']);
        }
      },
      (error) => {
        console.error(error);
        this.toastr.error('There was an error Signing Up!', error.ExceptionMessage);
      }
    );
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
  }

  takeDetailsButton() {
    this.takingDetails = true;
  }

  goBackToCredentialsButton() {
    this.takingDetails = false;
  }
}
