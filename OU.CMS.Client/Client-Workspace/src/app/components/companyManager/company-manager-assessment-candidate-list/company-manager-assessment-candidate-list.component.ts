import { Component, Input, OnInit } from '@angular/core';
import { NavbarTabs } from 'src/app/shared/enums/NavbarTabs';
import { CandidateTestDto } from 'src/app/shared/models/CandidateModels';
import { TestScoreDto } from 'src/app/shared/models/TestModels';
import { NavbarService } from 'src/app/shared/services/navbar.service';

@Component({
  selector: 'app-company-manager-assessment-candidate-list',
  templateUrl: './company-manager-assessment-candidate-list.component.html',
  styleUrls: ['./company-manager-assessment-candidate-list.component.css']
})
export class CompanyManagerAssessmentCandidateListComponent implements OnInit {

  @Input() candidateList: CandidateTestDto[];
  @Input() testScores: TestScoreDto[];

  constructor(
    private navbarService: NavbarService
  ) { }

  ngOnInit(): void {
    this.navbarService.setCurrentTab(NavbarTabs.Assessments);
  }

}
