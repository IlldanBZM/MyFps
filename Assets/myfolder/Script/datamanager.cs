using UnityEngine;

public class datamanager
{
    public static float m_volume = 0.5f;
    public static float m_mosen = 0.5f;

    private static int m_gamelevel = 1;
    public static int M_gamelevel
    {
        get { return m_gamelevel; }
    }

    public static readonly int m_maxlevel = 5;

    private static int m_gamescore = 0;
    public static int M_gamescore
    {
        get { return m_gamescore; }
    }

    private static float m_playtime;
    public static float M_playtime
    {
        get { return m_playtime; }
    }

    public static bool m_isnewlevel = false;

    public static void AddGameScore()
    {
        m_gamescore++;
    }

    public static void AddGameLevel()
    {
        m_gamelevel++;
    }

    public static void CalcuTime()//计时
    {
        m_playtime += Time.deltaTime;
    }

    public static void ReturnMenu()
    {
        m_playtime = 0;
        m_gamescore = 0;
        m_gamelevel = 1;
    }
}
