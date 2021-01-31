import { Component, OnInit } from '@angular/core';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-drives',
  templateUrl: './company-manager-drives.component.html',
  styleUrls: ['./company-manager-drives.component.css']
})
export class CompanyManagerDrivesComponent implements OnInit {

  constructor(
    private navbarService: NavbarService
  ) { }

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Drives);
  }

}
