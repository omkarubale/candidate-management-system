import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/api/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private accountService: AccountService) {}

  isAuthenticated: boolean = this.accountService.isAuthenticated;
  isCandidate: boolean = this.accountService.isCandidateLogin;

  ngOnInit(): void {
  }

}
