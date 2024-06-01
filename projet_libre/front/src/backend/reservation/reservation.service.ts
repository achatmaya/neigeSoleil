import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CoreHttpClientDelete } from 'src/core/http/services/core-http-client-delete.service';
import { CoreHttpClientGet } from 'src/core/http/services/core-http-client-get.service';
import { CoreHttpClientPatch } from 'src/core/http/services/core-http-client-patch.service';
import { CoreHttpClientPost } from 'src/core/http/services/core-http-client-post.service';
import { reservationCreate,reservation } from './reservation.interface';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

  export class ReservationService {

    constructor(
      private http : HttpClient,
      private httpGet: CoreHttpClientGet,
      private httpPost: CoreHttpClientPost,
      private httpPatch: CoreHttpClientPatch,
      private httpDelete: CoreHttpClientDelete
    ) {}
    addReservation(reservation: reservation): Observable<reservation> {
      return this.httpPost.post('reservation', reservation);
    }
    getReservation(id: number):Observable<reservation>{
      return this.httpGet.one(`reservation/${id}`)
    }
    updateReservation(reservation : reservation):Observable<reservation>{
      return this.httpPatch.patch(`reservation` , reservation)
    }

  listAllByOwner( UserId: number): Observable<reservation[]> {
    return this.httpGet.list(`reservation/Owner/${UserId}`);
  }

    delete(id: number): Observable<void> {
      return this.httpDelete.delete(`reservation/${id}`);
    }

  }
