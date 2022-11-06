package groups.common.utils;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class EnvironmentUtils {

    private static String applicationContext;
    private static String jwtSecret;
    private static int futureCreations;


    @Autowired
    public EnvironmentUtils(@Value("${application.context}") String applicationContext,
                            @Value("${jwt.secret}") String jwtSecret,
                            @Value("${future.creations}") int futureCreations) {

        EnvironmentUtils.jwtSecret = jwtSecret;
        EnvironmentUtils.applicationContext = applicationContext;
        EnvironmentUtils.futureCreations = futureCreations;
    }


    public static String getApplicationContext() {
        return applicationContext;
    }

    public static String getJwtSecret() {
        return jwtSecret;
    }

    public static int getFutureCreations() {
        return futureCreations;
    }
}
