import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  // TODO: get these values from back end
  isAuthenticated: boolean = true;
  isCandidate: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

}
