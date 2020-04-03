import { Photo } from './photo';

export interface User {
    // Replicate what we return for our user, UserForListDto, and UserforDetailDto

    id: number;  // Can use lowercase here, and uppercase in C#
    string: string;
    knownAs: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    playerPosition: string;

    description?: string; // Optional, must put these after required params
    rinkAmenities?: string;
    rinkInquiries?: string;
    photos?: Photo[];
}
