import { Injectable } from '@angular/core';
import {
    CanActivate, Router,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    CanActivateChild,
    NavigationExtras,
    CanLoad, Route
} from '@angular/router';

@Injectable()
export class AuthFilter implements CanActivate, CanActivateChild, CanLoad {
    constructor(private router: Router) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        const url: string = state.url;
        return this.checkLogin(url);
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(route, state);
    }

    canLoad(route: Route): boolean {
        const url = `/${route.path}`;

        return this.checkLogin(url);
    }
    checkLogin(url: string): boolean {
        const navigationExtras: NavigationExtras = {
            queryParams: { returnUrl: url }
        };
        console.log('checkLogin');
        this.router.navigate(['/login'], navigationExtras);
        return false;
    }
}
