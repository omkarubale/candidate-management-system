import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/api/account.service';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private navbarService: NavbarService,
    private router: Router,
    private toastr: ToastrService
    ) {}

  isAuthenticated: boolean;
  isCandidate: boolean;
  currentTab: NavbarTabs;

  isAuthenticatedSubscription = this.accountService.isAuthenticatedChange.subscribe((value) => {
    this.isAuthenticated = value;
  });
  isCandidateLoginSubscription = this.accountService.isCandidateLoginChange.subscribe((value) => {
    this.isCandidate = value;
  });
  currentTabSubscription = this.navbarService.currentTabChange.subscribe((value) => {
    this.currentTab = value;
  });

  ngOnInit(): void {
    this.isAuthenticated = this.accountService.isAuthenticated;
    this.isCandidate = this.accountService.isCandidateLogin;
    this.currentTab = this.navbarService.getCurrentTab();
  }

  logout() {
    this.accountService.logout();
    this.toastr.success('You have signed out successfully.', 'Sign out Successful!');
    this.router.navigate(['/home']);
  }

  ngOnDestroy(): void {
    this.isAuthenticatedSubscription.unsubscribe();
    this.isCandidateLoginSubscription.unsubscribe();
    this.currentTabSubscription.unsubscribe();
  }

}
