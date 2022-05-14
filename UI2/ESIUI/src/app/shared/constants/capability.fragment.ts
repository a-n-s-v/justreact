import { gql } from 'apollo-angular';

export const BASIC_CAPABILITY_FIELDS_FRAGMENT = gql`
fragment BasicCapabilityFields on User {
 id fullName bio username image 
}`;

export const CAPABILITY_FIELDS_FRAGMENT = gql`
fragment BasicCapabilityFields on User {
 id fullName bio username image 
}`;