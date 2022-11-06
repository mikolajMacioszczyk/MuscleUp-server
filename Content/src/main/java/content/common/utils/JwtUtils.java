package content.common.utils;

import content.common.innerCommunicators.errors.AuthHeaderException;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import javax.servlet.http.HttpServletRequest;

import static content.common.utils.StringUtils.concatenate;
import static java.util.Objects.isNull;
import static org.springframework.http.HttpHeaders.AUTHORIZATION;

public class JwtUtils {

    private static final String BEARER_PREFIX = "Bearer ";

    public static String getRequestJwtToken() {

        RequestAttributes attributes = RequestContextHolder.currentRequestAttributes();
        HttpServletRequest request = ((ServletRequestAttributes) attributes).getRequest();

        String jwtToken = request.getHeader(AUTHORIZATION);

        if (isNull(jwtToken)) throw new AuthHeaderException();

        return ensureBearer(jwtToken);
    }

    private static String ensureBearer(String jwtToken) {

        return jwtToken.contains(BEARER_PREFIX)? jwtToken : concatenate(BEARER_PREFIX, jwtToken);
    }
}