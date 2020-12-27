import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GetCandidateDto } from 'src/app/shared/models/CandidateModels';

@Component({
  selector: 'app-company-manager-position-candidate-list',
  templateUrl: './company-manager-position-candidate-list.component.html',
  styleUrls: ['./company-manager-position-candidate-list.component.css']
})
export class CompanyManagerPositionCandidateListComponent implements OnInit {

  @Input() candidateList: GetCandidateDto[];

  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
  }

}
