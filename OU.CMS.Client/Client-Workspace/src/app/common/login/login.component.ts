import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/shared/api/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  constructor(private accountSerice: AccountService) {}

  ngOnInit(): void {}
}
