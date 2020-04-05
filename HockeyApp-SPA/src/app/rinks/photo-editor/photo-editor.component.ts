import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
// Step 11 CLOUD UPLOAD bring array of photos in(next add to html)
export class PhotoEditorComponent implements OnInit {
@Input() photos: Photo[];
@Output() getMemberPhotoChange = new EventEmitter<string>();

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  baseUrl = environment.apiUrl;
  currentMain: Photo;

  constructor(private authservice: AuthService, private userService: UserService, private alertify: AlertifyService ) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader ({
      url: this.baseUrl + 'users/' + this.authservice.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token') ,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        // Response is a string by default, parse to JSON
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };

        this.photos.push(photo);
        if (photo.isMain) {
          this.authservice.changeMemberPhoto(photo.url);
          this.authservice.currentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authservice.currentUser));
        }
      }
    };
  }

  setMainPhoto(photo: Photo) {
    this.userService
      .setMainPhoto(this.authservice.decodedToken.nameid, photo.id)
      .subscribe(
        () => {
        // Use array filter to find the main photo (returns a copy of photos array, but filters anything that doesn't match)
        this.currentMain = this.photos.filter(p => p.isMain === true)[0];
        this.currentMain.isMain = false;
        photo.isMain = true;
      // this.getMemberPhotoChange.emit(photo.url); // For output, or to parent component.  Emits an event
        this.authservice.changeMemberPhoto(photo.url);
        this.authservice.currentUser.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authservice.currentUser));
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePhoto(id: number){
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.userService.deletePhoto(this.authservice.decodedToken.nameid, id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id),1);
        this.alertify.success('Photo has been deleted.');
      }, error => {
        this.alertify.error('Unable to delete photo.');
      });
    });
  }
}
