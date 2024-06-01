import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/backend/user/auth.service';
import { User } from 'src/backend/user/user.interface';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent {
  isLoginMode = true;
  username = '';
  password = '';
  lastName = '';
  firstName = '';
  email = '';
  phoneNumber = '';
  birthDate = '';
  address = '';
  addressComplement = '';
  postalCode = '';
  city = '';
  country = '';
  currentUser: User | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isOwner = false;

  constructor(private authService: AuthService, private router: Router) {
    this.authService.currentUser$.subscribe(user => this.currentUser = user);
   }

   onSwitchMode() {
    this.isLoginMode = !this.isLoginMode;
    this.errorMessage = null;
    this.successMessage = null;
  }

  onSubmit() {
    this.errorMessage = null;
    this.successMessage = null;

    if (this.isLoginMode) {
      this.authService.login(this.username, this.password).subscribe({
        next: response => {
          localStorage.setItem('token', response.token);
          this.router.navigate(['/']);
          this.authService.loadCurrentUser();
        },
        error: error => {
          this.handleError(error);
        }
      });
    } else {
      const registerData = {
        username: this.username,
        password: this.password,
        lastName: this.lastName,
        firstName: this.firstName,
        email: this.email,
        phoneNumber: this.phoneNumber,
        birthDate: this.birthDate,
        address: this.address,
        addressComplement: this.addressComplement,
        postalCode: this.postalCode,
        city: this.city,
        country: this.country,
        role: this.isOwner ? 'ROLE_OWNER' : 'ROLE_USER'
      };

      this.authService.register(registerData).subscribe({
        next: response => {
          this.isLoginMode = true;
          this.successMessage = 'Inscription réussie, veuillez vous connecter.';
        },
        error: error => {
          this.handleError(error);
        }
      });
    }
  }

  handleError(error: any) {
    if (error.status === 400) {
      this.errorMessage = 'Nom d\'utilisateur ou mot de passe invalide.';
    } else if (error.status === 401) {
      this.errorMessage = 'Identifiants incorrects. Veuillez réessayer.';
    } else if (error.status === 409) {
      this.errorMessage = 'Nom d\'utilisateur déjà existant. Veuillez en choisir un autre.';
    } else {
      this.errorMessage = 'Une erreur inattendue est survenue. Veuillez réessayer plus tard.';
    }
  }
}
