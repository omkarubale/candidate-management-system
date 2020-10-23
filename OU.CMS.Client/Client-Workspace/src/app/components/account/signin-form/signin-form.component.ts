import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/api/account.service';
import { SignInDto } from 'src/app/shared/models/AccountModels';
import { UserType } from 'src/app/shared/enums/UserType';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-signin-form',
  templateUrl: './signin-form.component.html',
  styleUrls: ['./signin-form.component.css'],
})
export class SigninFormComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  signInCredentials: SignInDto = new SignInDto();
  @Input('isCandidateLogin') isCandidateLogin: boolean;
  userTypeTag: string;

  ngOnInit(): void {
    this.resetForm();
    this.signInCredentials.UserType = this.isCandidateLogin ? UserType.Candidate : UserType.Management;
    this.userTypeTag = this.isCandidateLogin ? 'Candidate' : 'Company Manager';
  }

  signIn(form: SignInDto) {
    this.accountService.login(form).subscribe(
      (result) => {
        if (form.UserType === UserType.Candidate) {
          this.router.navigate(['/candidate-dashboard']);
        } else {
          this.router.navigate(['/companyManager-dashboard']);
        }
      },
      (error) => {
        console.error(error);
        this.toastr.error(error.message, 'There was an error Signing In!');
      }
    );
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
  }
}
