import { Component, OnInit } from '@angular/core';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-candidate-dashboard',
  templateUrl: './candidate-dashboard.component.html',
  styleUrls: ['./candidate-dashboard.component.css']
})
export class CandidateDashboardComponent implements OnInit {

  constructor(
    private navbarService: NavbarService
  ) { }

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Dashboard);
  }

}
