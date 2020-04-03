import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { RinkEditComponent } from '../rinks/rink-edit/rink-edit.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<RinkEditComponent> {
    canDeactivate(component: RinkEditComponent) {
        if (component.editForm.dirty) {
            return confirm('You have not saved changes to your profile.  Do you want to leave without finishing?');
        }
        return true;
    }
}
