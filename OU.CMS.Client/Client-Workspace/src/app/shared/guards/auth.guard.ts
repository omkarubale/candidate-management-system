import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../api/account.service';
import { UserInfo } from '../models/AuthenticationModels';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private accountService: AccountService
) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const userInfo = this.accountService.userInfo;

      if (userInfo && this.checkExpirationDate(userInfo.ExpiresOn)) {
        // logged in so return true
        return true;
      } else {
        this.accountService.logout();
        this.router.navigate(['/home']);
        return false;
      }
  }

  private checkExpirationDate(expiresOn : any) : boolean {
    var utcDate = new Date().toISOString();
    return utcDate < expiresOn;
  }
}
