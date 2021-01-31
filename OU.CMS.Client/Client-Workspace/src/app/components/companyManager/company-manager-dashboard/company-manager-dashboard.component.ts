import { Component, OnInit } from '@angular/core';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-dashboard',
  templateUrl: './company-manager-dashboard.component.html',
  styleUrls: ['./company-manager-dashboard.component.css']
})
export class CompanyManagerDashboardComponent implements OnInit {

  constructor(
    private navbarService: NavbarService
  ) { }

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Dashboard);
  }

}
