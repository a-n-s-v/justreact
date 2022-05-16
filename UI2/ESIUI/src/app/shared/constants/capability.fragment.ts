import { gql } from 'apollo-angular';

export const BASIC_CAPABILITY_FIELDS_FRAGMENT = gql`
fragment BasicCapabilityFields on Capability {
 id name description
}`;

export const CAPABILITY_FIELDS_FRAGMENT = gql`
fragment BasicCapabilityFields on Capability {
 id name description
}`;
