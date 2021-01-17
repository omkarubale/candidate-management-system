import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { UserService } from 'src/app/shared/api/user.service';
import { UserInfo } from 'src/app/shared/models/AuthenticationModels';
import { UserDto } from 'src/app/shared/models/UserModels';

@Component({
  selector: 'app-candidate-my-profile',
  templateUrl: './candidate-my-profile.component.html',
  styleUrls: ['./candidate-my-profile.component.css'],
})
export class CandidateMyProfileComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private userService: UserService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router
  ) {}

  userDetails: UserDto;

  ngOnInit(): void {
    this.userService
      .getIdentityUser()
      .subscribe((data) => {
        this.userDetails = data;
      });
  }

  // User
  editUser(form: UserDto) {
    this.userService.saveIdentityUser(form).subscribe(
      (result) => {
        this.toastr.success(
          'Your details were saved successfully.',
          'My Profile Updated!'
        );
      },
      (error) => {
        console.error(error);
        this.toastr.error(
          error.message,
          'There was an error saving your details!'
        );
      }
    );
  }

  getShortName() {
    return (this.userDetails.FirstName != "" ? this.userDetails.FirstName[0].toUpperCase() : "") +
    (this.userDetails.LastName != "" ? this.userDetails.LastName[0].toUpperCase() : "");
  }
}
