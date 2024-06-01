import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from 'src/backend/user/auth.service';
import { User } from 'src/backend/user/user.interface';
import { UserService } from 'src/backend/user/user.service';

@Component({
  selector: 'app-user-profil',
  templateUrl: './user-profil.component.html',
  styleUrls: ['./user-profil.component.scss'],
})
export class UserProfilComponent {
  currentUser: User | null = null;
  isEditingProfile: boolean = false;

  constructor(
    private authService: AuthService,
    private userService: UserService
  ) {
    this.authService.currentUser$.subscribe(user => this.currentUser = user);
  }

  ngOnInit(): void {
    this.authService.loadCurrentUser();
  }

  onEditProfile(): void {
    this.isEditingProfile = true;
  }

  onSaveProfile(): void {
    if (this.currentUser) {
      this.userService.update(this.currentUser).subscribe({
        next: (updatedUser) => {
          this.currentUser = updatedUser;
          this.isEditingProfile = false;
          this.authService.checkUsernameChange(updatedUser.username);
        },
        error: (error) => console.error(error)
      });
    }
  }

  onCancelEdit(): void {
    this.isEditingProfile = false;
    this.authService.loadCurrentUser(); // Reload the user to discard any unsaved changes
  }
}
