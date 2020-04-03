import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RinkListComponent } from './rinks/rink-list/rink-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { RinkDetailComponent } from './rinks/rink-detail/rink-detail.component';
import { RinkDetailResolver } from './_resolver/rink-details.resolver';
import { RinkListResolver } from './_resolver/rink-list-resolver';


export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path: 'rinks', component: RinkListComponent,
                                 resolve: {users: RinkListResolver}},
            {path: 'rinks/:id', component: RinkDetailComponent,
                                 resolve: {user: RinkDetailResolver}}, // Add the id param we need for the loadUser method
            {path: 'messages', component: MessagesComponent},
            {path: 'lists', component: ListsComponent},
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
