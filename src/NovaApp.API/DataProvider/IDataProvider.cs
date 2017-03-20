﻿using System.Collections.Generic;
using NovaApp.API.DataObjects;

namespace NovaApp.API.DataProvider
{
    public interface IDataProvider
    {
        List<TransactionDataObject> GetTransactions();
        List<TransactionDataObject> GetTransactionByUserId(int userId);
        TransactionDataObject GetTransactionById(int id);
        TransactionDataObject AddTransaction(TransactionDataObject transaction);

        List<PlayerDataObject> GetPlayers();
        PlayerDataObject GetPlayerById(int id);
        List<PlayerDataObject> GetPlayerByClubId(int id);
        PlayerDataObject AddPlayer(PlayerDataObject player);
        PlayerDataObject PatchPlayer(int playerId, PlayerDataObject player);
        PlayerDataObject PutPlayer(int playerId, PlayerDataObject player);
        void DeletePlayer(int playerId);


        List<ClubDataObject> GetClubs();
        ClubDataObject GetClubById(int id);
        ClubDataObject AddClub(ClubDataObject club);
    }
}