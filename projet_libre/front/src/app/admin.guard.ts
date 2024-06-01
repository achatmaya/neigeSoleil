
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/backend/user/auth.service';
import { AuthenticationTokenService } from 'src/core/http/services/authentication.service';
import { UserGlobales } from 'src/core/user-global/user-global';

@Injectable({
  providedIn: 'root'
})

export class AdminGuard implements CanActivate {
  user:any;
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean > | boolean  {
      if (!this.authService.asRole('ROLE_ADMIN')) {
        this.router.navigate(['/home']);
        return false;
      }
      console.log("passe no IF");

      return true;
  }
}
