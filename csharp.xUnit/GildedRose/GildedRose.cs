using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;
    const int maxNonLedgendaryQuality = 50;
    private const int concertTenDayThreshold = 10;
    private const int concertFiveDayThreshold = 5;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            UpdateItemQuality(item);
        }
    }

    private static void UpdateItemQuality(Item item)
    {
        if (ItemIsLegendary(item))
        {
            return;
        }

        if (ItemImprovesInQualityBeforeSellDate(item))
        {
            if (IsBackStageConcertPass(item))
            {
                UpdateConcertPasses(item);
            }
            else
            {
                item.Quality++;
            }
        }
        else
        {
            if (item.Quality > 0)
            {
                item.Quality--;
            }
        }

        if (item.Quality > maxNonLedgendaryQuality)
        {
            item.Quality = maxNonLedgendaryQuality;
        }

        UpdateSellIn(item);

        if (item.SellIn < 0)
        {
            if (item.Name is "Aged Brie")
            {
                if (item.Quality < maxNonLedgendaryQuality)
                {
                    item.Quality++;
                }
            }
            else
            {
                if (item.Name is not "Backstage passes to a TAFKAL80ETC concert")
                {
                    if (item.Quality > 0)
                    {
                        item.Quality--;
                    }
                }
                else
                {
                    item.Quality = 0;
                }
            }
        }
    }

    private static bool IsBackStageConcertPass(Item item)
    {
        return item.Name is "Backstage passes to a TAFKAL80ETC concert";
    }

    private static bool ItemIsLegendary(Item item) => item.Name is "Sulfuras, Hand of Ragnaros";

    private static bool ItemImprovesInQualityBeforeSellDate(Item item)
    {
        return item.Name is "Aged Brie" or "Backstage passes to a TAFKAL80ETC concert";
    }

    private static void UpdateConcertPasses(Item item)
    {
        switch (item.SellIn)
        {
            case <= concertFiveDayThreshold: 
                item.Quality += 3;
                break;
            case <= concertTenDayThreshold: 
                item.Quality += 2; 
                break;
            default:
                item.Quality++;
                break;
        }
    }

    private static void UpdateSellIn(Item item)
    {
        item.SellIn--;
    }
}