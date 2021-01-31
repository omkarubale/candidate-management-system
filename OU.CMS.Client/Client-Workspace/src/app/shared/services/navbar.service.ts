import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NavbarTabs } from '../enums/NavbarTabs';

@Injectable()
export class NavbarService {
  private currentTab: NavbarTabs = NavbarTabs.Dashboard;
  currentTabChange: Subject<NavbarTabs> = new Subject<NavbarTabs>();

  constructor() {
  }

  getCurrentTab(): NavbarTabs {
    return this.currentTab;
  }

  setCurrentTab(tab: NavbarTabs) {
    this.currentTab = tab;
    this.currentTabChange.next(tab);
  }
}
