import { Component, Input, OnInit } from '@angular/core';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { GetCandidateDto } from 'src/app/shared/models/CandidateModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-position-candidate-list',
  templateUrl: './company-manager-position-candidate-list.component.html',
  styleUrls: ['./company-manager-position-candidate-list.component.css']
})
export class CompanyManagerPositionCandidateListComponent implements OnInit {

  @Input() candidateList: GetCandidateDto[];

  constructor(
    private navbarService: NavbarService
  ) { }

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Positions);
  }

}
