import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/api/user.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { UserInfo } from 'src/app/shared/models/AuthenticationModels';
import { UserDto } from 'src/app/shared/models/UserModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-candidate-my-profile',
  templateUrl: './candidate-my-profile.component.html',
  styleUrls: ['./candidate-my-profile.component.css'],
})
export class CandidateMyProfileComponent implements OnInit {
  constructor(
    private navbarService: NavbarService,
    private userService: UserService,
    private toastr: ToastrService,
    private cd: ChangeDetectorRef,
    private modalService: NgbModal,
    private router: Router
  ) {}

  userDetails: UserDto;

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.MyProfile);

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
