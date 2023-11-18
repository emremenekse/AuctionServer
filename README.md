# Auction Application

## Overview
Our Auction application is an interactive platform that allows users to create and participate in auctions under various organizations. This platform features organizations managed by company admins and auctions managed by sellers.

## Features

- **User Login:** Membership system with user login. Password reset feature for forgotten passwords.
- **Dashboard:** Organizations are accessible via a user-specific dashboard and are listed by their names.
- **Organization Management:** Only admins have the authority to add organizations.
- **Auctions:** Each organization contains multiple ongoing or completed auctions. Auctions can be added by sellers.
- **Auction Details:** Includes the auction's end time, name, photo, description, current highest bid, and starting price.
- **Live Bid Tracking:** Users can see bids placed during the auction in real time.
- **Money Transfer:** Upon completion of the auction, the money transfer process is carried out by the seller.
- **User Balance:** Product providers and auction winners can check their balance from their profile page.

## Technical Details

- **Frontend:** Angular.
- **Backend:** .NET Core.
- **Real-Time Operations:** SignalR.
- **Database:** MsSQL.
