import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CandidateTestDto } from 'src/app/shared/models/CandidateModels';
import { TestScoreDto } from 'src/app/shared/models/TestModels';

@Component({
  selector: 'app-company-manager-assessment-candidate-list',
  templateUrl: './company-manager-assessment-candidate-list.component.html',
  styleUrls: ['./company-manager-assessment-candidate-list.component.css']
})
export class CompanyManagerAssessmentCandidateListComponent implements OnInit {

  @Input() candidateList: CandidateTestDto[];
  @Input() testScores: TestScoreDto[];

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
  }

}
