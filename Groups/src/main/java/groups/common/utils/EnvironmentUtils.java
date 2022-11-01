package groups.common.utils;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class EnvironmentUtils {

    private static String applicationContext;
    private static String jwtSecret;


    @Autowired
    public EnvironmentUtils(@Value("${application.context}") String applicationContext,
                            @Value("${jwt.secret}") String jwtSecret) {

        EnvironmentUtils.jwtSecret = jwtSecret;
        EnvironmentUtils.applicationContext = applicationContext;
    }


    public static String getApplicationContext() {

        return applicationContext;
    }

    public static String getJwtSecret() {

        return jwtSecret;
    }
}
