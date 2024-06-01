import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, tap } from 'rxjs';
import { CoreHttpClientGet } from 'src/core/http/services/core-http-client-get.service';
import { CoreHttpClientPost } from 'src/core/http/services/core-http-client-post.service';
import { User } from './user.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  public currentUser$: Observable<User | null> = this.currentUserSubject.asObservable();
  private currentUser: User | null = null;

  constructor( private httpGet: CoreHttpClientGet,    private httpPost: CoreHttpClientPost) {
  }

  login(username: string, password: string): Observable<{ token: string }> {
    return this.httpPost.postLogin('authenticate', { username, password }).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
        this.loadCurrentUser();
      })
    );
  }

  register(user: Partial<User>): Observable<User> {
    return this.httpPost.postLogin('register', user);
  }

  loadCurrentUser(): void {
    this.getCurrentUser().subscribe(user => {console.log(user);
     this.currentUserSubject.next(user)} );
  }

  getCurrentUser(): Observable<User> {
    return this.httpGet.one('me');
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  isAdmin(): boolean {
    const currentUser = this.currentUserSubject.value;
    return currentUser ? currentUser.role === 'ROLE_ADMIN' : false;
  }

  asRoleAsync(role: string): boolean {
    const currentUser = this.currentUserSubject.value;
    //console.log(currentUser);
    return currentUser ? currentUser.role === role : false;
  }

  async asRole(role: string): Promise<boolean> {
    const currentUser = await this.getCurrentUserAsync();
    //console.log(currentUser);
    return currentUser ? currentUser.role === role : false;
  }

  setCurrentUser(user: User): void {
    this.currentUserSubject.next(user);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  checkUsernameChange(newUsername: string) {
    const currentUser = this.currentUserSubject.value;
    if (currentUser && currentUser.username !== newUsername) {
      this.logout();
      alert('Nom d\'utilisateur modifié. Veuillez vous reconnecter.');
    }
  }

  private getCurrentUserAsync(): Promise<User | null> {
    const currentUser = this.currentUserSubject.value;
    if (currentUser) {
      return Promise.resolve(currentUser);
    }
    return this.httpGet.one('me').toPromise().then(user => {
      this.currentUserSubject.next(user);
      return user;
    }).catch(() => null);
  }
}
