import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CoreHttpClientDelete } from 'src/core/http/services/core-http-client-delete.service';
import { CoreHttpClientGet } from 'src/core/http/services/core-http-client-get.service';
import { CoreHttpClientPatch } from 'src/core/http/services/core-http-client-patch.service';
import { CoreHttpClientPost } from 'src/core/http/services/core-http-client-post.service';
import { ApartmentCreate , Apartment} from './appartement.interface';

@Injectable({
  providedIn: 'root'
})
export class AppartementService {
  constructor(
    private httpGet: CoreHttpClientGet,
    private httpPost: CoreHttpClientPost,
    private httpPatch: CoreHttpClientPatch,
    private httpDelete: CoreHttpClientDelete
  ) {}

  addAppartement(apartment: ApartmentCreate): Observable<ApartmentCreate> {
    return this.httpPost.post('apartment', apartment);
  }

  updateAppartement(apartmentId: string, apartment: Partial<ApartmentCreate>): Observable<any> {
    return this.httpPatch.patch('apartment/'+apartmentId, apartment);
  }

  getlistAppartement(): Observable<Apartment[]> {
    return this.httpGet.list('apartment');
  }

  getAppartementById(id: number): Observable<Apartment> {
    return this.httpGet.one('apartment/'+id);
  }
  // Other methods (commented out)
  // listByUser(userId: number): Observable<Appartment[]> {
  //   return this.httpGet.list(`Appartment?userId=${userId}`);
  // }

  // getOne(id: number): Observable<Apartment> {
  //   return this.httpGet.one(`Appartment/${id}`);
  // }

  // update(Appartment: Appartment): Observable<Appartment> {
  //   return this.httpPatch.patch(`Appartment/${Appartment.id}`, Appartment);
  // }

  // delete(id: number): Observable<void> {
  //   return this.httpDelete.delete(`Appartment/${id}`);
  // }
}
