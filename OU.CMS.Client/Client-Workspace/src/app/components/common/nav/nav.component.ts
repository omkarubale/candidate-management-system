import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
    ) {}

  isAuthenticated: boolean;
  isCandidate: boolean;

  currentTab: NavbarTabs = NavbarTabs.Dashboard;

  isAuthenticatedSubscription = this.accountService.isAuthenticatedChange.subscribe((value) => {
    this.isAuthenticated = value;
  });
  isCandidateLoginSubscription = this.accountService.isCandidateLoginChange.subscribe((value) => {
    this.isCandidate = value;
  });

  ngOnInit(): void {
    this.isAuthenticated = this.accountService.isAuthenticated;
    console.log("account Service isAuthenticated: ", this.isAuthenticated);
    this.isCandidate = this.accountService.isCandidateLogin;
  }

  logout() {
    this.accountService.logout();
    this.toastr.success('You have signed out successfully.', 'Sign out Successful!');
    this.router.navigate(['/home']);
  }

  changeActiveTab(tab: number) {
    this.currentTab = tab;
  }

  ngOnDestroy(): void {
    this.isAuthenticatedSubscription.unsubscribe();
    this.isCandidateLoginSubscription.unsubscribe();
  }

}
