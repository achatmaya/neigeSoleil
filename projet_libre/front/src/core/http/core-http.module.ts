import { AuthenticationTokenService } from 'src/core/http/services/authentication.service';
import { NgModule } from '@angular/core';
import { HttpCoreConfig } from './services/config.services';
import { CoreHttpHeaders } from './services/core-http-headers.service';
import { CoreHttpClientGet } from './services/core-http-client-get.service';
import { CoreHttpClientDelete } from './services/core-http-client-delete.service';
import { CoreHttpClientPost } from './services/core-http-client-post.service';
import { CoreHttpClientPatch } from './services/core-http-client-patch.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';

@NgModule({
    imports: [

    ],
    providers: [
        HttpCoreConfig,
        CoreHttpHeaders,
        CoreHttpClientGet,
        CoreHttpClientDelete,
        CoreHttpClientPost,
        CoreHttpClientPatch,
        AuthenticationTokenService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
       ]
})

export class CoreHttpModule {
}
