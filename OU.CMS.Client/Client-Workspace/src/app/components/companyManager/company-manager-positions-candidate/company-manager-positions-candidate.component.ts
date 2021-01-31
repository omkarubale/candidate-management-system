import { Component, OnInit } from '@angular/core';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-positions-candidate',
  templateUrl: './company-manager-positions-candidate.component.html',
  styleUrls: ['./company-manager-positions-candidate.component.css']
})
export class CompanyManagerPositionsCandidateComponent implements OnInit {

  constructor(
    private navbarService: NavbarService
  ) { }

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Positions);
  }

}
