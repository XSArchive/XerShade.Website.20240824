﻿namespace XerShade.Website.Core.Services.Interfaces;

public interface IPopulationService
{
    void PopulateFactories();
    Task PopulateFactoriesAsync();
}
