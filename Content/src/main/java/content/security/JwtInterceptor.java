package content.security;

import com.auth0.jwt.JWT;
import com.auth0.jwt.algorithms.Algorithm;
import content.common.innerCommunicators.errors.AuthHeaderException;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.HandlerInterceptor;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import java.util.List;
import java.util.concurrent.atomic.AtomicBoolean;

import static content.common.utils.EnvironmentUtils.getJwtSecret;
import static java.util.Objects.isNull;
import static org.springframework.http.HttpHeaders.AUTHORIZATION;

@Component
public class JwtInterceptor implements HandlerInterceptor {

    private static final String BEARER_PREFIX = "Bearer ";
    private static final List<String> AUTHORIZE_FREE = List.of(
            "/swagger",
            "/exercise/all-active",
            "/workout/all-active"
    );


    @Override
    public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler) {

        AtomicBoolean freeAccess = new AtomicBoolean(false);

        AUTHORIZE_FREE.forEach(path -> {

            if (request.getRequestURL().toString().contains(path)) {

                freeAccess.set(true);
            }
        });

        return freeAccess.get() || authorize(request);
    }

    private boolean authorize(HttpServletRequest request) {

        String jwtToken = request.getHeader(AUTHORIZATION);

        if (isNull(jwtToken)) throw new AuthHeaderException();

        if (jwtToken.contains(BEARER_PREFIX)) jwtToken = jwtToken.replace(BEARER_PREFIX, "");

        String secret = getJwtSecret();
        Algorithm algorithm = Algorithm.HMAC512(secret);

        try {

            algorithm.verify(JWT.decode(jwtToken));
        }
        catch (Exception e) {

            throw new UnauthorizedException();
        }

        return true;
    }
}
