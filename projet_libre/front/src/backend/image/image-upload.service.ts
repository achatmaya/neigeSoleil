import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {

  constructor(private http: HttpClient) { }

  uploadImage(imageFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', imageFile, imageFile.name);

    return this.http.post('http://localhost:8090/File/upload', formData);
  }
}
