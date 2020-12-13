import { Component, Input, OnInit } from '@angular/core';
import { CandidateTestDto } from 'src/app/shared/models/CandidateModels';

@Component({
  selector: 'app-company-manager-assessment-candidate-list',
  templateUrl: './company-manager-assessment-candidate-list.component.html',
  styleUrls: ['./company-manager-assessment-candidate-list.component.css']
})
export class CompanyManagerAssessmentCandidateListComponent implements OnInit {

  @Input() candidateList: CandidateTestDto[];

  constructor() { }

  ngOnInit(): void {
  }

}
