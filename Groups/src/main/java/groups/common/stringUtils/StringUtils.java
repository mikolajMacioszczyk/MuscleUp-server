package groups.common.stringUtils;

import static java.util.Objects.isNull;

public class StringUtils {

    public static boolean isNullOrEmpty(String target) {

        return isNull(target) || target.isEmpty();
    }
}
