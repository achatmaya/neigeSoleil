import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/backend/user/auth.service';
import { User } from 'src/backend/user/user.interface';
import { UserService } from 'src/backend/user/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  selectedUser: User | undefined;
  isEditingUser: boolean = false;
  isEditingAddress: number | null = null;
  isAddingAddress: boolean = false;
  currentUser: User | null = null;

  constructor(
    private userService: UserService,
    private authService: AuthService
  ) {
    this.authService.currentUser$.subscribe(user => this.currentUser = user);
  }

  ngOnInit(): void {
    this.userService.listAll().subscribe({
      next: (data: User[]) => this.users = data,
      error: (error) => console.error(error)
    });
  }

  onSelect(user: User): void {
    this.selectedUser = user;
    this.isEditingUser = false;
    this.isEditingAddress = null;
    this.isAddingAddress = false;
  }



  onEditUser(): void {
    this.isEditingUser = true;
  }

  onSaveUser(): void {
    if (this.selectedUser) {
      this.userService.update(this.selectedUser).subscribe({
        next: (updatedUser) => {
          this.isEditingUser = false;
          // Vérifier si le nom d'utilisateur a changé
          if (this.currentUser && this.currentUser.id === updatedUser.id) {
            this.authService.checkUsernameChange(updatedUser.username);
          }
        },
        error: (error) => console.error(error)
      });
    }
  }

  onDeleteUser(): void {
    if (this.selectedUser) {
      this.userService.delete(this.selectedUser.id).subscribe({
        next: () => {
          this.users = this.users.filter(u => u.id !== this.selectedUser!.id);
          this.selectedUser = undefined;
        },
        error: (error) => console.error(error)
      });
    }
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }
}
